import axios from 'axios';

export default class ContributorsServices {
    
    getcontributors() {
      return axios.get("https://localhost:44384/api/v1/contributors");
    }

    postcontributors(contributors = {}) {
      return axios.post("https://localhost:44384/api/v1/contributors", contributors);
    }

    putcontributors(contributorsId, contributors = {}) {
      return axios.put(`https://localhost:44384/api/v1/contributors/${contributorsId}`, contributors);
    }

    deletecontributors(contributorsId) {
      return axios.delete(`https://localhost:44384/api/v1/contributors/${contributorsId}`);
    }
}