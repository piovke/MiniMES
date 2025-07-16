<script setup>

import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
const route = useRoute();
const product = ref(null);

async function fetchProduct() {
  const id = route.params.Id;
  const response = await fetch('http://localhost:5001/Products/Details/' + id);
  if(response.ok) {
    product.value = await response.json();
  }
  else{
    console.error(response);
  }
}

onMounted(() => {
  fetchProduct();
})
const name = ref(null);
const description = ref(null);
async function updateProduct(id){
  var response =await fetch('http://localhost:5001/Machines/UpdateProduct?id=' + id, {
    method: 'PUT',
    headers:{'Content-Type': 'application/json'},
    body: JSON.stringify({
      name: name.value,
      description: description.value,
    })
  })
  console.log(response)
  await fetchProduct()
}

</script>

<template>
  <h1>{{product?.name}}</h1>
  <p>{{product?.description}}</p>

  <h3>Active orders:</h3>
  <div v-if="product?.orders.length===0">No orders</div>
  <ul>
    <li v-for="order in product?.orders">
      {{order.code}}
      <router-link :to="`/Orders/Details/${order.id}`">
        <button>order details</button>
      </router-link>
    </li>
  </ul>
  <br><br><br>
  Edit this product:<br>
  <form @submit.prevent="updateProduct(product.id)">
    name:  <input v-model="name" type="text" placeholder="new product name..."/><br>
    description:<input v-model="description" type="text" placeholder="new product description..."/><br>
    <button type="submit">Update</button>

  </form>
</template>

<style scoped>

</style>