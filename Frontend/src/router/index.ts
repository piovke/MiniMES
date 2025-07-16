import { createRouter, createWebHistory } from 'vue-router'

import MachinesView from '../../views/Machines.vue'
import ProductsView from '../../views/Products.vue'
import OrdersView from '../../views/Orders.vue'
import ProcessesView from '../../views/Processes.vue'
import MachineDetailsView from '../../views/MachineDetails.vue'
import ProductDetailsView from '../../views/ProductDetails.vue'
import OrderDetailsView from '../../views/OrderDetails.vue'


const routes = [
    { path: '/Machines', component: MachinesView },
    { path: '/Products', component: ProductsView },
    { path: '/Orders', component: OrdersView },
    { path: '/Processes', component: ProcessesView },
    { path: '/Machines/Details/:Id', component: MachineDetailsView },
    { path: '/Products/Details/:Id', component: ProductDetailsView },
    { path: '/Orders/Details/:Id', component: OrderDetailsView },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
