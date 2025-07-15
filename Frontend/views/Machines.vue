<script setup>
import {ref, onMounted} from 'vue'
const machines = ref([])

async function fetchMachines() {
  const response = await fetch('http://localhost:5001/Machines/ShowMachines')
  if (response.ok){
    machines.value = await response.json()
  }
}
const name = ref('')
const description = ref('')
async function addMachine() {
  try {
    const response = await fetch('http://localhost:5001/Machines/AddMachine', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        name: name.value,
        description: description.value
      })
    })

    if (!response.ok) {
      throw new Error('Błąd dodawania maszyny')
    }

    console.log('Maszyna dodana!')
  } catch (error) {
    console.error(error)
  }
  await fetchMachines()
}
async function deleteMachine(id) {
  
  const response = await fetch('http://localhost:5001/Machines/DeleteMachine?id=' + id, {
    method: 'DELETE',
  });
  if (!response.ok) {
    throw new Error('Failed to delete machine')
  }
  await fetchMachines()
}

const confirmDelete = (id) => {
  if (window.confirm("are you sure to delete?")){
    deleteMachine(id)
  }
}

onMounted(async () => {
  fetchMachines()
})
</script>

<template>
  <div class="container">
    <div class="machineList">
      <h1>Lista Maszyn</h1>
      <h3 v-if="machines.length == 0">no machines</h3>
      <ul>
        <li v-for="machine in machines" :key="machine.id">
          {{ machine.name }}
          <router-link :to="`/Machines/Details/${machine.id}`">
            <button>Machine detalis</button>
          </router-link>
          <button @click="confirmDelete(machine.id)">Delete</button>
          <br>
          Current orders: {{machine.orderIds.length}}
        </li>
      </ul>
    </div>
    <div class="machineAdd">
      <h2>Dodaj maszynę</h2>
      <form @submit.prevent="addMachine">
        <input v-model="name" placeholder="Nazwa" />
        <input v-model="description" placeholder="Opis" />
        <button type="submit">Dodaj</button>
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
</style>
