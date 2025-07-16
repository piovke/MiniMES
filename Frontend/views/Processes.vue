<script setup>
import {ref, onMounted} from "vue";
const processes = ref([]);

async function fetchOrders() {
  const response = await fetch('http://localhost:5001/Processes/ShowProcesses');
  if (response.ok) {
    processes.value = await response.json();
  }
}
onMounted(() => {
  fetchOrders();
})
</script>

<template>
  <h1>processes:</h1>
  <h3 v-if="processes.length===0">no processes</h3>
  <ul v-if="processes">
    <li v-for="order in processes">
      {{ order?.code }}
      <router-link :to="`/Process/Details/${process.id}`">
        <button>details</button>
      </router-link>
      <button @click="">delete</button>
    </li>
  </ul>
</template>

<style scoped>

</style>