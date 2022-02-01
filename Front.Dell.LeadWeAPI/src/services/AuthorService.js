import axios from 'axios';

export default class AuthorServices {
    
  getAuthor() {
    return axios.get("https://localhost:44384/api/v1/Author");
  }

  postAuthor(author = {}) {
    return axios.post("https://localhost:44384/api/v1/Author", author);
  }

  putAuthor(authorName, author = {}) {
    return axios.put(`https://localhost:44384/api/v1/Author/${authorName}`, author);
  }

  deleteAuthor(authorName) {
    return axios.delete(`https://localhost:44384/api/v1/Author/${authorName}`);
  }
}