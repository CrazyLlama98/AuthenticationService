import {
  OpenAPIClientAxios
} from 'openapi-client-axios'

const api = new OpenAPIClientAxios({
  definition: 'http://localhost:32770/swagger/v1/swagger.json',
  axiosConfigDefaults: {
    baseURL: 'http://localhost:32770',
    withCredentials: true
  }
});

export default api;