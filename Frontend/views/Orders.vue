<script setup>
import {ref, onMounted} from "vue";
const orders = ref([]);

async function fetchOrders() {
  const response = await fetch('http://localhost:5001/Orders/ShowOrders');
  if (response.ok) {
    orders.value = await response.json();
  }
}
onMounted(() => {
  fetchOrders();
})
</script>

<template>
  <h1>Orders:</h1>
  <h3 v-if="orders.length===0">no orders</h3>
  <ul v-if="orders">
    <li v-for="order in orders">
      {{ order?.code }}
      <router-link :to="`/Orders/Details/${order.id}`">
        <button>details</button>
      </router-link>
      <button @click="">delete</button>
    </li>
  </ul>
</template>

<style scoped>

</style>