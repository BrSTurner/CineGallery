import { ensureWriteAccess } from './auth.js';

export async function openProtectedPage({ status, formAlert, forbidden, back }) {
  status.hidden = false;
  status.textContent = 'Checking access…';

  try {
    const access = await ensureWriteAccess();
    if (access.status === 'redirecting') {
      status.textContent = 'Redirecting to sign in…';
      return null;
    }

    if (access.status === 'forbidden') {
      status.hidden = true;
      forbidden.hidden = false;
      back.hidden = false;
      return null;
    }

    status.hidden = true;
    return access;
  } catch (error) {
    status.hidden = true;
    formAlert.hidden = false;
    formAlert.textContent = error.message || 'Could not confirm access.';
    back.hidden = false;
    return null;
  }
}
