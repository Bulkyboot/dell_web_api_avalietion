import axios from 'axios'

export default class ContributorsServices {
  getContributors() {
    return axios.get('https://localhost:44373/api/v1/Contributors')
  }
  postContributors(contributors = {}) {
    return axios.post('https://localhost:44373/api/v1/contributors', contributors)
  }
}
