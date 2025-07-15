<script setup>
import {ref, onMounted} from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const machine = ref(null)
const name = ref("")
const description = ref("")

async function fetchMachine() {
  const id = route.params.Id
  const response = await fetch('http://localhost:5001/Machines/Details/' + id)
  if (response.ok) {
    machine.value = await response.json()
    name.value = machine.value.name
    description.value = machine.value.description
  } else {
    console.error('Błąd podczas pobierania maszyny')
  }
}

onMounted(() => {
  fetchMachine()
})

async function updateMachine(id){
  var response =await fetch('http://localhost:5001/Machines/UpdateMachine?id=' + id, {
    method: 'PUT',
    headers:{'Content-Type': 'application/json'},
    body: JSON.stringify({
      name: name.value,
      description: description.value,
    })
  })
  console.log(response)
  await fetchMachine()
}
</script>

<template>
  <h1>{{machine?.name}}</h1>
  <p>{{machine?.description}}</p>
  
  <h3>Active orders:</h3>
  <div v-if="machine?.orders.length===0">No orders</div>
  
  <ul>
    <li v-for="order in machine?.orders">
    {{order.code}}
<!--      <router-link :to="`/Orders/Details/${machine?.order.id}`">-->
<!--        <button>s</button>-->
<!--      </router-link>-->
    </li>
  </ul>
  <br><br><br>
  Edit this machine:<br>
  <form @submit.prevent="updateMachine(machine.id)">
  name:  <input v-model="name" type="text" placeholder="new machine name..."/><br>
  descriptoin:<input v-model="description" type="text" placeholder="new machine description..."/><br>
  <button type="submit">Update</button>
    
  </form>
</template>

<style scoped>

</style>