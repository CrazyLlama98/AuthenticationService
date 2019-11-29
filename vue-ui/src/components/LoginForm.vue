<template>
  <v-card elevation="10" class="py-5 px-3">
    <v-card-title primary-title class="display-1 justify-center">Sign In</v-card-title>
    <v-row justify="center">
      <v-col cols="10">
        <ValidationObserver tag="v-form" @submit.prevent="submit" v-slot="{ invalid }">
          <ValidationTextField
            v-model="username"
            name="User Name"
            rules="required"
            label="User Name"
          />
          <ValidationTextField
            v-model="password"
            name="Password"
            rules="required|min:8"
            label="Password"
            type="password"
          />
          <v-checkbox v-model="rememberMe" label="Remember Me" type="checkbox"></v-checkbox>
          <v-btn
            rounded
            block
            color="primary"
            :loading="isLoading"
            :disabled="invalid"
            type="submit"
          >
            Login
            <template v-slot:loader>
              <span class="custom-loader">
                <v-icon dark>fas fa-sync</v-icon>
              </span>
            </template>
          </v-btn>
        </ValidationObserver>
      </v-col>
    </v-row>
    <v-divider></v-divider>
    <v-row class="text-center" justify="center">
      <v-col cols="10">
        <span class="caption">Or Sign In using:</span>
      </v-col>
      <v-col cols="3">
        <v-btn rounded block color="primary" outlined>
          <v-icon small>fab fa-facebook</v-icon>
        </v-btn>
      </v-col>
      <v-col cols="3">
        <v-btn rounded block color="primary" outlined>
          <v-icon small>fab fa-google</v-icon>
        </v-btn>
      </v-col>
    </v-row>
    <v-divider></v-divider>
    <v-row class="text-center" justify="center">
      <v-col cols="10">
        <router-link
          class="caption"
          :to="{ path: '/register', query: $route.query }"
        >Don't have an account already? Sign up here!</router-link>
      </v-col>
    </v-row>
    <NotificationSnackbar
      v-model="notification.isActive"
      :color="notificationColor"
      :timeout="2000"
      :message="notification.message"
    />
  </v-card>
</template>

<script>
import api from "@/api";
import ValidationTextField from "@/components/ValidationTextField.vue";
import NotificationSnackbar from "@/components/NotificationSnackbar.vue";

export default {
  name: "LoginForm",
  components: {
    ValidationTextField,
    NotificationSnackbar
  },
  data: () => ({
    username: null,
    password: null,
    rememberMe: false,
    notification: {
      isActive: false,
      message: null,
      isError: false
    },
    isLoading: false
  }),
  computed: {
    notificationColor() {
      return this.notification.isError ? "error" : "success";
    }
  },
  methods: {
    async submit() {
      try {
        this.isLoading = true;
        await api.init();
        var client = await api.getClient();
        var response = await client.login(
          { version: 1, ...this.$route.query },
          {
            username: this.username,
            password: this.password,
            rememberMe: this.rememberMe
          }
        );
        if (response.status == 204) {
          window.location = this.$route.query.ReturnUrl;
        } else {
          this.notification.isError = true;
          this.notification.isActive = true;
          this.notification.message = "An unexpected error has occurred. Please try again later.";
        }
      } catch (error) {
        this.notification.message = error.response.data.errors.default[0];
        this.notification.isError = true;
        this.notification.isActive = true;
      } finally {
        this.isLoading = false;
      }
    }
  }
};
</script>