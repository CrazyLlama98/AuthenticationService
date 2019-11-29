<template>
  <v-container fluid class="pa-0 ma-0">
    <v-row justify="center" class="wrapper" align="center">
      <v-col cols="11" sm="7" md="5" lg="3">
        <router-view />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "home"
};
</script>

<style lang="scss" scoped>
.wrapper {
  height: 100vh;
  width: 100%;
  margin: 0;
  padding: 0;
  background: url("../assets/login-background.png");
  background-repeat: no-repeat;
  background-size: cover;
}
</style>
