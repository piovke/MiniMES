<script setup>
import {ref, onMounted} from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const machine = ref(null)

async function fetchMachine() {
  const id = route.params.Id
  const response = await fetch('http://localhost:5001/Machines/Details/' + id)
  if (response.ok) {
    machine.value = await response.json()
  } else {
    console.error('Błąd podczas pobierania maszyny')
  }
}

onMounted(() => {
  fetchMachine()
})
</script>

<template>
  <h1>{{machine?.name}}</h1>
  <p>{{machine?.description}}</p>
  
  <h3>Active orders:</h3>
  <div v-if="machine?.orders.length===0">No orders</div>
  
  <ul>
    <li v-for="order in machine?.orders">
    {{order.code}}
    </li>
  </ul>
</template>

<style scoped>

</style>