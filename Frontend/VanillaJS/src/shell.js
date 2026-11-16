import { accountLabel, login, logout } from './auth.js';

export function bindShell(access) {
  const accountName = document.querySelector('#account-name');
  const signIn = document.querySelector('#sign-in');
  const signOut = document.querySelector('#sign-out');

  if (access.account) {
    accountName.textContent = accountLabel(access.account);
    accountName.hidden = false;
    signOut.hidden = false;
    signIn.hidden = true;
  } else {
    accountName.hidden = true;
    signOut.hidden = true;
    signIn.hidden = false;
  }

  signIn.addEventListener('click', () => {
    login().catch(showAuthError);
  });
  signOut.addEventListener('click', () => {
    logout().catch(showAuthError);
  });
}

function showAuthError(error) {
  const alert = document.querySelector('#form-alert') ?? document.querySelector('#status');
  if (!alert) {
    return;
  }

  alert.hidden = false;
  alert.className = 'alert alert-danger';
  alert.textContent = error?.message || 'Sign-in failed.';
}
