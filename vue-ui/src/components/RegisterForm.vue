<template>
  <v-card elevation="10" class="py-5 px-3">
    <v-card-title primary-title class="display-1 justify-center">Sign Up</v-card-title>
    <v-row justify="center">
      <v-col cols="10">
        <ValidationObserver
          ref="observer"
          tag="v-form"
          @submit.prevent="submit"
          v-slot="{ invalid }"
        >
          <ValidationTextField
            v-model="username"
            name="User Name"
            rules="required"
            label="User Name"
          />
          <ValidationTextField v-model="email" name="Email" rules="required|email" label="Email" />
          <ValidationTextField
            v-model="password"
            name="Password"
            vid="password"
            label="Password"
            type="password"
            rules="required|min:8"
          />
          <ValidationTextField
            v-model="confirmPassword"
            name="Password"
            label="Confirm Password"
            type="password"
            rules="required|confirmed:password"
          />
          <v-btn
            rounded
            block
            color="primary"
            :loading="isLoading"
            :disabled="invalid"
            type="submit"
          >
            Register
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
        <span class="caption">Or Sign Up using:</span>
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
          :to="{ path: '/login', query: $route.query }"
        >You already have an account? Sign in here!</router-link>
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
  name: "RegisterForm",
  components: {
    ValidationTextField,
    NotificationSnackbar
  },
  data: () => ({
    email: null,
    username: null,
    password: null,
    confirmPassword: null,
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
        var response = await client.createAccount(
          { version: 1 },
          {
            username: this.username,
            password: this.password,
            email: this.email
          }
        );
        if (response.status == 204) {
          this.notification.message =
            "The registration was successful and you can go to login page now";
          this.notification.isError = false;
          this.clearForm();
        } else {
          this.notification.message =
            "An unexpected error has occurred. Please try again later.";
          this.notification.isError = true;
        }
      } catch (error) {
        this.notification.message = error.response.data.errors.default[0];
        this.notification.isError = true;
      } finally {
        this.isLoading = false;
        this.notification.isActive = true;
      }
    },
    clearForm() {
      this.email = null;
      this.username = null;
      this.password = null;
      this.confirmPassword = null;
      this.$refs.observer.reset();
    }
  }
};
</script>