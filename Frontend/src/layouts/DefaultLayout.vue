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

.main-block {
  padding: 40px;
}

.sub-block {
  padding-top: 20px;
  margin-top: 10px;
  cursor: default !important;
  border-radius: 20px;
}

.mobile-hidden {
  display: block !important;
}
.mobile-display {
  display: none !important;
}
@media (max-width: 768px) {
  .mobile-display {
    display: block !important;
  }
  .mobile-hidden {
    display: none !important;
  }
}

/* Button Style（MainPage 等仍暫用） */
.button-common {
  margin: 10px;
  padding: 15px 30px;
  text-align: center;
  text-transform: uppercase;
  transition: 0.5s;
  background-size: 200% auto;
  color: white;
  border-radius: 10px;
  display: block;
  border: 0px;
  font-weight: 700;
  box-shadow: 0px 0px 14px -7px #f09819;
  background-image: linear-gradient(45deg, #ff512f 0%, #f09819 51%, #ff512f 100%);
  cursor: pointer;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-common:hover {
  background-position: right center;
  color: #fff;
  text-decoration: none;
}

.button-common:active {
  transform: scale(0.95);
}

.flex-btn {
  display: flex;
  align-items: center;
  justify-content: center;
}

@font-face {
  font-family: "madoufmg";
  src: url("@/assets/fonts/madoufmg.ttf") format("truetype");
}
</style>
