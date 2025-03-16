import React, { useEffect, useState } from 'react';
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface Stock {
  symbol: string;
  price: number;
}

interface StockChartProps {
  stock: Stock;
  color?: string;
}

const StockChart: React.FC<StockChartProps> = ({ stock, color = 'rgba(75, 192, 192, 1)' }) => {
  const [priceHistory, setPriceHistory] = useState<number[]>([]);

  useEffect(() => {
    setPriceHistory(prev => {
      const newHistory = [...prev, Math.round(stock.price)];
      if (newHistory.length > 10) {
        newHistory.shift();
      }
      return newHistory;
    });
  }, [stock]);

  const data = {
    labels: Array.from({ length: 10 }, (_, i) => `${i + 1}s ago`),
    datasets: [{
      label: stock.symbol,
      data: priceHistory,
      borderColor: color,
      backgroundColor: color.replace('1)', '0.2)'),
      tension: 0.1,
      fill: false,
    }],
  };

  const options = {
    responsive: true,
    plugins: {
      legend: {
        display: false,
      },
      title: {
        display: true,
        text: `${stock.symbol} Stock Price`,
      },
    },
    scales: {
      y: {
        beginAtZero: false,
        ticks: {
          stepSize: 100,
        },
        grid: {
          color: 'rgba(0, 0, 0, 0.1)',
        }
      }
    }
  };

  return <Line data={data} options={options} />;
};

export default StockChart;
