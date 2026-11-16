import 'bootstrap/dist/css/bootstrap.min.css';
import '../hidden.css';
import { getMovie, updateMovie } from '../api.js';
import { initAuth, isRedirecting, refreshAccess } from '../auth.js';
import { openProtectedPage } from '../gate.js';
import { bindShell } from '../shell.js';

const status = document.querySelector('#status');
const formAlert = document.querySelector('#form-alert');
const forbidden = document.querySelector('#forbidden');
const back = document.querySelector('#back');
const form = document.querySelector('#movie-form');
const nameInput = document.querySelector('#name');

await initAuth();
if (!isRedirecting()) {
  bindShell(await refreshAccess());
  const id = new URLSearchParams(location.search).get('id');

  if (!id) {
    formAlert.hidden = false;
    formAlert.textContent = 'Movie id is missing.';
    back.hidden = false;
  } else {
    const access = await openProtectedPage({ status, formAlert, forbidden, back });
    if (access) {
      await showMovie(id);
    }
  }
}

async function showMovie(id) {
  let movie;
  try {
    movie = await getMovie(id);
  } catch (error) {
    formAlert.hidden = false;
    formAlert.textContent = error.message || 'Could not load the movie.';
    back.hidden = false;
    return;
  }

  nameInput.value = movie.name;
  form.hidden = false;
  form.addEventListener('submit', async (event) => {
    event.preventDefault();
    const button = form.querySelector('button[type="submit"]');
    const name = new FormData(form).get('name').trim();
    button.disabled = true;
    formAlert.hidden = true;

    try {
      await updateMovie({ id: movie.id, name, rowVersion: movie.rowVersion });
      location.assign('index.html');
    } catch (error) {
      if (error.code === 'redirecting') {
        status.hidden = false;
        status.textContent = 'Redirecting to sign in…';
        form.hidden = true;
        return;
      }

      formAlert.hidden = false;
      formAlert.textContent = error.message || 'Could not update the movie.';
      button.disabled = false;
    }
  });
}
