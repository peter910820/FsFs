export const fileIcon = (name: string) => {
  const lower = name.toLowerCase();
  if ([".png", ".jpg", ".jpeg", ".gif", ".webp"].some((ext) => lower.endsWith(ext))) return "mdi-image-outline";
  if ([".zip", ".rar", ".7z", ".tar.gz"].some((ext) => lower.endsWith(ext))) return "mdi-folder-zip-outline";
  if ([".go", ".py", ".fs", ".cs", ".ts", ".js"].some((ext) => lower.endsWith(ext))) return "mdi-code-braces";
  if ([".mp4", ".mkv", ".webm"].some((ext) => lower.endsWith(ext))) return "mdi-video-outline";
  if ([".mp3", ".wav", ".flac"].some((ext) => lower.endsWith(ext))) return "mdi-music-note";
  if (lower.endsWith(".xp3")) return "mdi-cog-outline";
  return "mdi-file-outline";
};

export const openStaticFile = (path: string) => {
  window.location.href = `${import.meta.env.VITE_STATIC_FILE_DOMAIN}/${path}`;
};
