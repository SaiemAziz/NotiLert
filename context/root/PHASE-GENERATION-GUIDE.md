# Phase-Wise Folder Generation Guide

This file is the instruction set for generating version folders **one phase at a time**, matching the exact structure, detail level, and conventions used in Phase 0 and Phase 1. Give an agent this file + `root/roadmap.md` and it should be able to generate the next phase without any further explanation from you.

Read `root/RULES.md` first if it hasn't been read yet this session — this file assumes that structure is already understood.

---

## When to Use This File

Trigger this process when a phase is fully complete (every version folder in it is `status: complete` in `versions/INDEX.md`) and you're ready to scaffold the next phase's version folders.

---

## Procedure for the Agent

1. **Determine the target phase.** Open `root/roadmap.md`, find the next phase after the last one already scaffolded in `versions/`, and read its full step table (Goal / Core Concept / Key Terms / Validation for each step).
2. **Determine the starting version number.** Look at `versions/INDEX.md` — continue numbering sequentially from the last version present. Never renumber or touch existing version folders.
3. **Decide folder slugs.** For each roadmap step in the phase, create a 3-5 word kebab-case slug summarizing it (matching the style already used: `discord-telegram-channels`, `durable-broker-swap`, etc.). One roadmap step = one version folder, unless the user has explicitly said otherwise for this phase.
4. **For each step in the phase, generate exactly these 5 files**, following the schema below precisely — match the structure, tone, and level of detail already present in the Phase 0/Phase 1 folders (check an existing one, e.g. `v08-discord-telegram-channels/00-plan.md`, as the reference example before writing):

   - `00-plan.md` — fully written now, not a blank template. Must include: frontmatter (version, title, roadmap_steps, status: not-started, started/completed blank), Goal (inlined, in your own words from the roadmap's Goal + Core Concept columns — 1-2 paragraphs), Inherited Context (a few bullets describing what the *previous* version in sequence left behind, inferred logically from what's been built so far — not copied verbatim from any file, reasoned out), Sub-Steps (4-6 concrete, commit-sized checklist items breaking the step into an implementation sequence), Theory to Know Going In (bullet list from the roadmap's Key Terms + Core Concept), Validation (copied directly from the roadmap's Validation column, as checkboxes).
   - `01-manual-steps.md` — fully written now. Frontmatter (version only), a checklist of developer-only actions (account creation, installs, dashboard config, credential generation, manual verification) relevant to this specific step, and a blank "Notes" section at the end.
   - `02-outcome.md`, `03-interview-prep.md`, `04-release-notes.md` — copy unmodified from `templates/`, only patching the `version: v##` frontmatter field. Leave all body content blank/templated — these are filled only when the version is actually completed, never during scaffolding.

5. **Update `versions/INDEX.md`** — append one row per new version folder, status `not-started`, Completed column blank. Do not alter existing rows.
6. **Do not touch `CURRENT.md`** unless the user explicitly asks to jump straight into the new phase — scaffolding a phase is not the same as starting work on it.
7. **Report back** a short summary: how many version folders were created, their numbers/titles, and ask the user to spot-check one or two plan files before continuing — same as was done for Phase 0/1.

---

## What NOT to Do

- Don't scaffold more than one phase at a time unless explicitly asked.
- Don't pre-fill `02-outcome.md`, `03-interview-prep.md`, or `04-release-notes.md` with real content — they stay blank until version completion, no exceptions.
- Don't re-read or re-summarize the entire `roadmap.md` in your response back to the user — just confirm what was generated.
- Don't change the numbering, slugs, or content of any already-existing version folder.

---

## Minimal Prompt That Should Trigger This Correctly

> "Generate the next phase's version folders, following `context/root/PHASE-GENERATION-GUIDE.md`."

That's sufficient — the agent should be able to find the right phase, the right starting number, and the right format from the guide plus the existing folders as reference, without you re-explaining anything.
