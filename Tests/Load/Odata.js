import http from 'k6/http';

export let options = {
    vus: 5,
    duration: '30s'
}
export default function () {
    let url = http.url`http://localhost:5005/odata/roles?select=name&expand=Permissions($select=name)`

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    http.get(url, params);
}
