<template>
  <div class="main-style">
    <NavBar />
    <v-main>
      <v-container fluid class="pa-6">
        <router-view />
      </v-container>
    </v-main>
  </div>
</template>

<script setup lang="ts">
import NavBar from "@/components/NavBar.vue";
</script>

<style>
.main-style {
  min-height: 100vh;
  min-width: 600px;
  font-family: "madoufmg";
}

@font-face {
  font-family: "madoufmg";
  src: url("@/assets/fonts/madoufmg.ttf") format("truetype");
}
</style>
