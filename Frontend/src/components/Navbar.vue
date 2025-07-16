<template>
  <nav class="navbar">
    <router-link to="/Machines" class="nav-link">Machines</router-link>
    <router-link to="/Products" class="nav-link">Products</router-link>
    <router-link to="/Orders" class="nav-link">Orders</router-link>
    <router-link to="/Processes" class="nav-link">Processes</router-link>
  </nav>
</template>

<style scoped>
.navbar {
  display: flex;
  gap: 1rem;
  background-color: #333;
  padding: 1rem;
  margin: 0;
}

.nav-link {
  color: white;
  text-decoration: none;
}

.nav-link:hover {
  text-decoration: underline;
}
</style>
