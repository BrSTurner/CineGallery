import 'bootstrap/dist/css/bootstrap.min.css';
import '../hidden.css';
import { getMovies } from '../api.js';
import { initAuth, isRedirecting, refreshAccess } from '../auth.js';
import { bindShell } from '../shell.js';

const status = document.querySelector('#status');
const scopeNotice = document.querySelector('#scope-notice');
const empty = document.querySelector('#empty');
const table = document.querySelector('#movie-table');
const rows = document.querySelector('#movie-rows');
const actionsHeader = document.querySelector('#actions-header');
const createLink = document.querySelector('#create-movie');

await initAuth();
if (!isRedirecting()) {
  const access = await refreshAccess();
  bindShell(access);
  status.hidden = false;

  try {
    const movies = await getMovies();
    status.hidden = true;

    if (access.authorized) {
      createLink.hidden = false;
      actionsHeader.hidden = false;
    }

    if (access.signedIn && !access.authorized) {
      scopeNotice.hidden = false;
    }

    if (movies.length === 0) {
      empty.hidden = false;
    } else {
      table.hidden = false;
      for (const movie of movies) {
        rows.append(movieRow(movie, access.authorized));
      }
    }
  } catch (error) {
    status.hidden = false;
    status.className = 'alert alert-danger';
    status.textContent = error.message || 'Could not load movies.';
  }
}

function movieRow(movie, authorized) {
  const row = document.createElement('tr');
  const name = document.createElement('td');
  name.textContent = movie.name;
  row.append(name);

  if (!authorized) {
    return row;
  }

  const actions = document.createElement('td');
  actions.className = 'text-end';

  const edit = document.createElement('a');
  edit.className = 'btn btn-sm btn-outline-secondary';
  edit.href = `edit.html?id=${encodeURIComponent(movie.id)}`;
  edit.textContent = 'Edit';

  const remove = document.createElement('a');
  remove.className = 'btn btn-sm btn-outline-danger ms-2';
  remove.href = `delete.html?id=${encodeURIComponent(movie.id)}`;
  remove.textContent = 'Delete';

  actions.append(edit, remove);
  row.append(actions);
  return row;
}
