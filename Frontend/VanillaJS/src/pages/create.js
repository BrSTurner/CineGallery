import 'bootstrap/dist/css/bootstrap.min.css';
import '../hidden.css';
import { createMovie } from '../api.js';
import { initAuth, isRedirecting, refreshAccess } from '../auth.js';
import { openProtectedPage } from '../gate.js';
import { bindShell } from '../shell.js';

const status = document.querySelector('#status');
const formAlert = document.querySelector('#form-alert');
const forbidden = document.querySelector('#forbidden');
const back = document.querySelector('#back');
const form = document.querySelector('#movie-form');

await initAuth();
if (!isRedirecting()) {
  bindShell(await refreshAccess());
  const access = await openProtectedPage({ status, formAlert, forbidden, back });
  if (access) {
    form.hidden = false;
    form.addEventListener('submit', async (event) => {
      event.preventDefault();
      const button = form.querySelector('button[type="submit"]');
      const name = new FormData(form).get('name').trim();
      button.disabled = true;
      formAlert.hidden = true;

      try {
        await createMovie(name);
        location.assign('index.html');
      } catch (error) {
        if (error.code === 'redirecting') {
          status.hidden = false;
          status.textContent = 'Redirecting to sign in…';
          form.hidden = true;
          return;
        }

        formAlert.hidden = false;
        formAlert.textContent = error.message || 'Could not create the movie.';
        button.disabled = false;
      }
    });
  }
}
