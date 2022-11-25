import http from 'k6/http';
import { uuidv4 } from "https://jslib.k6.io/k6-utils/1.0.0/index.js";

export let options = {
    vus: 5,
    duration: '1m'
}
export default function () {
    let userId = uuidv4();
    let url = http.url`http://localhost:5181/checkaccess/userId/${userId}/permissionId/24ADA77D-9A7B-4A21-A77D-0856B59A8D1A`

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    http.get(url, params);
}
