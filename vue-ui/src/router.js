import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'

Vue.use(Router)

export default new Router({
  routes: [{
    path: '/',
    name: 'home',
    redirect: 'login',
    component: Home,
    children: [{
      path: 'login',
      name: 'login',
      component: () => import('./components/LoginForm.vue')
    }, {
      path: 'register',
      name: 'register',
      component: () => import('./components/RegisterForm.vue')
    }]
  }],
  mode: 'history'
})