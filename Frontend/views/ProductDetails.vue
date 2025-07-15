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

</script>

<template>
  <h1>{{product?.name}}</h1>
  <p>{{product?.description}}</p>

  <h3>Active orders:</h3>
  <div v-if="product?.orders.length===0">No orders</div>
  <ul>
    <li v-for="order in product?.orders">
      {{order.code}}
    </li>
  </ul>
</template>

<style scoped>

</style>