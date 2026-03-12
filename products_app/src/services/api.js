import axios from "axios";

const api=axios.create({
    baseURL:"https://localhost:7234/api"
});

export default api;