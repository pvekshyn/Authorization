import http from 'k6/http';

export let options = {
    vus: 5,
    duration: '30s'
}
export default function () {
    let url = http.url`http://localhost:5006/graphql`

    const query = `
    {
      roles
       { 
           name, 
           permissions { name }
       }
   }`;

   const headers = {
     'Content-Type': 'application/json',
   };

    http.post(url, JSON.stringify({ query: query }), {headers: headers});
}
