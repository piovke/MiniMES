<script setup>
import {ref, onMounted} from "vue"
import {useRouter} from "vue-router"
const products = ref([])

async function fetchProducts() {
  const response = await fetch('http://localhost:5001/Products/ShowProducts')
  if (response.ok) {
    products.value = await response.json()
  }
}

onMounted(async() => {
  await fetchProducts()
})

const name = ref("")
const description = ref("")
async function addProduct() {
  if(name.value === '' ||  description.value === ''){
    alert("fill the form ")
    return
  }
  const response = await fetch('http://localhost:5001/Products/AddProduct', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      name: name.value,
      description: description.value
    })
  })
  await fetchProducts()
}

async function confirmDelete(id){
  if(window.confirm("Are you sure you want to delete this product?")) {
    deleteProduct(id)
  }
}
async function deleteProduct(id) {
  const response = await fetch('http://localhost:5001/Products/DeleteProduct?id='+id, {
    method: 'DELETE',
  });
  if(!response.ok) {
    throw new Error("Failed to delete product")
  }
  await fetchProducts()
}

</script>

<template>
  <div class="container">
  <div class="machineList">
    <h1>Product List</h1>
    <h3 v-if="products.length == 0">no products</h3>
    <ul>
      <li v-for="product in products" :key="product.id">
        {{ product.name }}
        <router-link :to="`/Products/Details/${product.id}`">
          <button>Product detalis</button>
        </router-link>
        <button @click="confirmDelete(product.id)">Delete</button>
        <br>
        Current orders: {{product.orderIds.length}}
      </li>
    </ul>
  </div>
  <div class="machineAdd">
    <h2>Add Product</h2>
    <form @submit.prevent="addProduct">
      <input v-model="name" placeholder="Name" />
      <br>
      <textarea v-model="description" placeholder="Description" />
      <br>
      <button type="submit">Add</button>
    </form>
  </div>
  </div>
</template>

<style scoped>
.container {
  display: grid;
  grid-template-columns: 70% 30%;
  gap: 16px;
  height: 100%;
}

.machineList {
  background-color: cadetblue;
  padding: 16px;
}

.machineAdd {
  background-color: lightgray;
  padding: 16px;
}
textarea{
  width: 90%;
  height: 100px;
}
input[type=text]{
  width: 90%;
}
</style>