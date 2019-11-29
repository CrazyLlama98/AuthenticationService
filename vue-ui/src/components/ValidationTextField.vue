<template>
  <ValidationProvider :name="name" :rules="rules" v-slot="{ errors }" :vid="vid">
    <v-text-field
      :value="value"
      @input="updateValue($event)"
      :label="label"
      :error-messages="errors"
      :type="type"
    ></v-text-field>
  </ValidationProvider>
</template>

<script>
export default {
  name: "ValidationTextField",
  props: ["value", "name", "label", "rules", "vid", "type"],
  methods: {
    updateValue(value) {
      this.$emit("input", value);
    }
  }
};
</script>