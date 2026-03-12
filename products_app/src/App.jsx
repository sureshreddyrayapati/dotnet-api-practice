import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Hello from './hello'
import ProductList from './Components/Products_GetAll'


function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <Hello/>
      <ProductList/>
    </>
  )
}

export default App
