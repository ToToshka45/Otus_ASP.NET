import React, { useState } from 'react';

const Counter = () => {
  // Используем хук useState для управления состоянием счетчика
  const [count, setCount] = useState(0);

  // Функция для увеличения счетчика на 1
  const increment = () => {
    setCount(count + 1);
  };

  // Функция для уменьшения счетчика на 1
  const decrement = () => {
    setCount(count - 1);
  };

  return (
    <div>
      <h1>Счетчик: {count}</h1>
      <button onClick={increment}>Увеличить</button>
      <button onClick={decrement}>Уменьшить</button>
    </div>
  );
};

export default Counter;