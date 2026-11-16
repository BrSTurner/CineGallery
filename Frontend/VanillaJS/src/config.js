const tenantId = '00000000-0000-0000-0000-000000000000';
const clientId = '00000000-0000-0000-0000-000000000000';

export const config = {
  clientId,
  tenantId,
  redirectUri: window.location.origin,
  apiScope: `api://${clientId}/Movies.Write`,
  scopeName: 'Movies.Write',
  apiBaseUrl: 'https://localhost:7229/api/v1.0'
};
