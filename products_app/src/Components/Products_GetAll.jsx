import { useEffect, useState } from "react";
import api from "../services/api";

function ProductList(){

    const [products, setProducts] = useState([]);

    useEffect(()=>{
        api.get("/products")
        .then((response)=>{
            setProducts(response.data.data);
        })
        .catch((error)=>{
            console.error("Error fetching products:", error);
        });
    },[]);

    return (
        <div>
            <h2>Products</h2>
            <ul>
                {products.map((product) => (
                    <li key={product.id}>{product.name}</li>
                ))}
            </ul>
        </div>
    );
}

export default ProductList;