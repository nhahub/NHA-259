const connection = new signalR.HubConnectionBuilder()
    .withUrl("/locationHub")
    .build();

connection.start().then(() => {
    console.log("Truck simulation connected.");

    setInterval(() => {
        let lat = 30.0444 + (Math.random() * 0.02);
        let lng = 31.2357 + (Math.random() * 0.02);

        connection.invoke("SendLocation", lat, lng);
    }, 3000);

}).catch(err => console.error(err));
