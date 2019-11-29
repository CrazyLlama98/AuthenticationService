<template>
  <v-snackbar class="mb-3" @input="closeSnackbar($event)" :value="value" bottom multi-line :color="color" :timeout="timeout">
    {{ message }}
    <v-btn dark text @click="closeSnackbar(false)">
      <v-icon>fas fa-times</v-icon>
    </v-btn>
  </v-snackbar>
</template>

<script>
export default {
  name: "NotificationSnackbar",
  props: [ 'color', 'timeout', 'value', 'message' ],
  methods: {
    closeSnackbar(value) {
      this.$emit('input', value);
    }
  }
};
</script>