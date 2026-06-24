<script setup lang="ts">
import { ref, onMounted } from "vue";
const errorMsg = ref("");

onMounted(async () => {
  errorMsg.value = sessionStorage.getItem("errorMsg") || "";
});
</script>

<template>
  <div class="row">
    <div class="col s12">
      <h1><font color="red">錯誤</font></h1>
    </div>
    <div class="col s12">
      <span>發生錯誤: {{ errorMsg }}</span>
    </div>
  </div>
</template>

<style scoped>
span {
  font-size: 25px;
}
</style>
