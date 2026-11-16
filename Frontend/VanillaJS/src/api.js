import { getAccessToken } from './auth.js';
import { config } from './config.js';

export class ApiError extends Error {
  constructor(status, message) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

async function readError(response) {
  const text = await response.text();
  if (!text) {
    return `Request failed (${response.status}).`;
  }

  try {
    const body = JSON.parse(text);
    if (typeof body === 'string' && body) {
      return body;
    }

    if (Array.isArray(body)) {
      return body.join(' ');
    }

    if (Array.isArray(body?.errors)) {
      return body.errors.join(' ');
    }

    if (body?.errors && typeof body.errors === 'object') {
      return Object.values(body.errors).flat().join(' ');
    }

    return body?.detail || body?.title || text;
  } catch {
    return text;
  }
}

async function send(path, options) {
  let response;
  try {
    response = await fetch(`${config.apiBaseUrl}${path}`, options);
  } catch {
    throw new ApiError(0, 'Could not reach the API. Check that it is running and that the browser trusts the development HTTPS certificate.');
  }

  if (!response.ok) {
    throw new ApiError(response.status, await readError(response));
  }

  if (response.status === 204) {
    return null;
  }

  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

function bearer(token) {
  return { Authorization: `Bearer ${token}` };
}

export function getMovies() {
  return send('/movies');
}

export function getMovie(id) {
  return send(`/movies/${encodeURIComponent(id)}`);
}

export async function createMovie(name) {
  const token = await getAccessToken();
  return send('/movies', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...bearer(token)
    },
    body: JSON.stringify({ name })
  });
}

export async function updateMovie(movie) {
  const token = await getAccessToken();
  return send('/movies', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      ...bearer(token)
    },
    body: JSON.stringify({
      id: movie.id,
      name: movie.name,
      rowVersion: movie.rowVersion
    })
  });
}

export async function deleteMovie(id, rowVersion) {
  const token = await getAccessToken();
  const query = new URLSearchParams({ rowVersion });
  return send(`/movies/${encodeURIComponent(id)}?${query}`, {
    method: 'DELETE',
    headers: bearer(token)
  });
}
