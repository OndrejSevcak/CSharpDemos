import { useEffect, useState } from 'react'
import * as signalR from '@microsoft/signalr'
import 'bootstrap/dist/css/bootstrap.min.css'; 
import './App.css'
import StockChart from './StockChart'

interface Stock {
  symbol: string
  price: number
}

const colors = [
  'rgba(75, 192, 192, 1)',
  'rgba(255, 99, 132, 1)',
  'rgba(54, 162, 235, 1)',
  'rgba(255, 206, 86, 1)',
  'rgba(153, 102, 255, 1)',
];

function App() {
  const [stocks, setStocks] = useState<Stock[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [disconnected, setDisconnected] = useState(false);

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

      connection.onclose(() => {
          setDisconnected(true);
      });

      // Cleanup function to stop the connection when the component unmounts
      return () => {
          connection.stop();
      };
  }, []);

  if (loading) {
      return <div>Loading...</div>;
  }

  if (disconnected) {
      return (
        <>
            <div>Disconnected. Click below to reconnect..</div>
            <button className='btn btn-primary w-30 mt-2' onClick={() => setDisconnected(false)}>Start</button>
        </>
      );
  }

  return (
    <>
    <div className='row mt-2'>
        <div className='col text-center'><h3>Stock price tracker App with .NET SignalR backend</h3></div>
    </div>
     <div className='d-flex justify-content-center w-100 mt-4'>
            <table className='table table-bordered w-50' border={1} cellPadding={10}>
                <thead>
                    <tr>
                        <th>Symbol</th>
                        <th>Price ($)</th>
                    </tr>
                </thead>
                <tbody>
                    {stocks.map(stock => (
                        <tr key={stock.symbol}>
                            <td>{stock.symbol}</td>
                            <td>{stock.price.toFixed(2)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
        <div className='d-flex w-100'>
        {stocks.map((stock, index) => (
            <div key={stock.symbol} style={{ width: "400px", margin: "10px" }}>
                <StockChart 
                stock={stock} 
                color={colors[index % colors.length]}
                />
            </div>
            ))}
        </div> 
        <div className='d-flex justify-content-evenly mt-5'>
            <button className='btn btn-warning w-30' onClick={() => setDisconnected(true)} >Cancel</button>
        </div>   
    </>
  );
};

export default App
