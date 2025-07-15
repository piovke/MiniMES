<script setup>
import {ref, onMounted} from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const order = ref(null)

async function fetchOrder() {
  const id = route.params.Id
  const response = await fetch('http://localhost:5001/Orders/Details/' + id)
  if (response.ok) {
    order.value = await response.json()
  } else {
    console.error('Loading order failed')
  }
}
  
  onMounted(() => {
    fetchOrder()
  })

</script>

<template>
  order code: {{order?.code}}<br>
  machine: {{order?.machine.name}}<br>
  product: {{order?.product.name}}<br>
  quantity: {{order?.quantity}}<br>
  Associated processes:<br>
  <p v-if="order?.processes.length===0">none</p>
  <ul>
    <li v-for="process in order?.processes">{{ process.serialNumber }}</li>
  </ul>
</template>

<style scoped>

</style>