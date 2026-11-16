import { InteractionRequiredAuthError, PublicClientApplication } from '@azure/msal-browser';
import { config } from './config.js';

const msal = new PublicClientApplication({
  auth: {
    clientId: config.clientId,
    authority: `https://login.microsoftonline.com/${config.tenantId}`,
    redirectUri: config.redirectUri,
    postLogoutRedirectUri: config.redirectUri
  },
  cache: {
    cacheLocation: 'sessionStorage'
  }
});

let ready;
let redirecting = false;

export function initAuth() {
  ready ??= start();
  return ready;
}

export function isRedirecting() {
  return redirecting;
}

async function start() {
  await msal.initialize();
  const result = await msal.handleRedirectPromise();

  if (result?.account) {
    msal.setActiveAccount(result.account);
  } else {
    const accounts = msal.getAllAccounts();
    if (accounts.length > 0) {
      msal.setActiveAccount(accounts[0]);
    }
  }

  const next = safeReturnPath(result?.state);
  if (next && next !== currentRoute()) {
    redirecting = true;
    location.assign(next);
  }
}

function currentRoute() {
  return canonical(`${location.pathname}${location.search}`);
}

function canonical(path) {
  const url = new URL(path, location.origin);
  const pathname = url.pathname === '/' ? '/index.html' : url.pathname;
  return `${pathname}${url.search}`;
}

function safeReturnPath(state) {
  if (!state || !state.startsWith('/') || state.startsWith('//')) {
    return null;
  }

  const url = new URL(state, location.origin);
  const allowed = ['/index.html', '/create.html', '/edit.html', '/delete.html'];
  if (url.origin !== location.origin || !allowed.includes(url.pathname === '/' ? '/index.html' : url.pathname)) {
    return null;
  }

  return canonical(`${url.pathname}${url.search}`);
}

export function getAccount() {
  return msal.getActiveAccount() ?? msal.getAllAccounts()[0] ?? null;
}

export function accountLabel(account) {
  return account?.name || account?.username || 'Signed in';
}

function loginScopes() {
  return ['openid', 'profile', config.apiScope];
}

export function login() {
  return msal.loginRedirect({
    scopes: loginScopes(),
    state: currentRoute()
  });
}

export function logout() {
  return msal.logoutRedirect({
    account: getAccount() ?? undefined,
    postLogoutRedirectUri: config.redirectUri
  });
}

function decodePayload(token) {
  const payload = token.split('.')[1];
  const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
  return JSON.parse(json);
}

export function tokenHasWriteScope(token) {
  if (!token) {
    return false;
  }

  try {
    const scope = decodePayload(token).scp;
    return typeof scope === 'string' && scope.split(' ').includes(config.scopeName);
  } catch {
    return false;
  }
}

export async function tryAcquireAccessToken() {
  await initAuth();
  const account = getAccount();
  if (!account) {
    return null;
  }

  try {
    const result = await msal.acquireTokenSilent({
      account,
      scopes: [config.apiScope]
    });
    return result.accessToken;
  } catch {
    return null;
  }
}

export async function refreshAccess() {
  await initAuth();
  const account = getAccount();
  if (!account) {
    return { signedIn: false, authorized: false, account: null };
  }

  const token = await tryAcquireAccessToken();
  return {
    signedIn: true,
    authorized: tokenHasWriteScope(token),
    account
  };
}

export async function ensureWriteAccess() {
  await initAuth();
  const account = getAccount();
  if (!account) {
    await login();
    return { status: 'redirecting' };
  }

  const request = {
    account,
    scopes: [config.apiScope],
    state: currentRoute()
  };

  try {
    const result = await msal.acquireTokenSilent(request);
    if (!tokenHasWriteScope(result.accessToken)) {
      return { status: 'forbidden', account };
    }

    return { status: 'ok', account, token: result.accessToken };
  } catch (error) {
    if (error instanceof InteractionRequiredAuthError) {
      await msal.acquireTokenRedirect(request);
      return { status: 'redirecting' };
    }

    throw error;
  }
}

export async function getAccessToken() {
  const access = await ensureWriteAccess();
  if (access.status === 'ok') {
    return access.token;
  }

  const error = new Error(
    access.status === 'forbidden'
      ? 'You are signed in, but this account does not have the Movies.Write scope.'
      : 'Redirecting to sign in.'
  );
  error.code = access.status;
  throw error;
}
