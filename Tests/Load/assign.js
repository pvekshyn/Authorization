import http from 'k6/http';
import { uuidv4 } from "https://jslib.k6.io/k6-utils/1.0.0/index.js";

export let options = {
    vus: 5,
    duration: '1m'
}
export default function () {
    const url = 'http://localhost:8080/assignment/assign';

    const payload = JSON.stringify({
        id: uuidv4(),
        userId: uuidv4(),
        roleId: '21C8DEA0-411F-4FA9-A95A-578D926F4AAD',
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    http.post(url, payload, params);
}
