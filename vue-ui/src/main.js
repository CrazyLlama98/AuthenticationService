import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import './plugins/vee-validate'
import router from './router'
import '@babel/polyfill'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import '@fortawesome/fontawesome-free/css/all.css'

Vue.config.productionTip = false

new Vue({
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')
