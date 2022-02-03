import axios from 'axios';

export default class ContributorsServices {
    
    getAddress() {
      return axios.get("https://localhost:44384/api/v1/address");
    }
}