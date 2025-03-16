## Stock Price Streaming App

![stock price app](https://github.com/OndrejSevcak/CSharpDemos/blob/master/StockPriceSimulator/stock_price_tracker_app.png)

This project demonstrates **real-time stock price streaming** using:
- **.NET 9 API** as the backend with a **Background Service** sending stock prices every second.
- **SignalR** for real-time communication between backend and frontend.
- **React** as the frontend to display stock price updates.
- **react-chartjs-2** for visualizing the last 10 prices of each stock.

### **Backend (C# .NET 9)**
- **ASP.NET Core Web API**
- **SignalR**
- **Background Service** for stock price simulation

### **Frontend (React + TypeScript)**
- **Vite** for fast React project setup
- **SignalR Client** for real-time updates
- **Bootstrap** for styling
- **react-chartjs-2** for stock price visualization

###  Features
- **Real-time stock price updates** (every second via SignalR).  
- **React frontend displays live stock prices**.  
- **Graphs for last 10 stock prices per stock** using Chart.js.  
- **Automatic reconnection in case of SignalR disconnects**.  


## 📈 SignalR Integration
### **Backend (C# SignalR Hub)**
```csharp
public class StockHub : Hub { }
```

- its an empty class because it acts just as a communication channel for clients
- if we would want to send or receive messages from the frontend, we would define custom methods inside this class

### **Frontend (React to SignalR Client)**
```tsx
 useEffect(() => {
      const connection = new signalR.HubConnectionBuilder()
          .withUrl("http://localhost:5111/stocksHub", {
            withCredentials: false,
            //skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
          }) // Adjust if API runs on a different port
          .withAutomaticReconnect()
          .build();

      connection.start()
          .then(() => {
              console.log("Connected to SignalR");
              setLoading(false);
              setDisconnected(false);
          })
          .catch(err => {
              console.error("SignalR Connection Error:", err);
              setError("Connection error. Please try again later.");
              setLoading(false);
          });

      //event handler for receiving stock updates - updates state
      connection.on("UpdateStockPrice", (updatedStocks: Stock[]) => {
          setStocks(updatedStocks);
      });
```

 **Author:** Ondrej Sevcak  


