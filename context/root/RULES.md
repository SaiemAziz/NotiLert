# NotiLert — Context System Rules

This file governs how any session (human or AI IDE) works inside `./context/`. Read this first, every time, before touching anything else in this folder.

## Folder Map

```
context/
  root/
    roadmap.md         Master roadmap (step definitions, concepts, validation criteria)
    RULES.md            This file
  versions/
    INDEX.md             Running table of all versions: status, dates, roadmap steps
    v##-slug/
      00-plan.md          Active during work. Detailed plan for this version, derived from roadmap.md.
      01-manual-steps.md   Active during work. Things the developer does by hand, not the agent.
      02-outcome.md         Generated ONLY at version completion.
      03-interview-prep.md  Generated ONLY at version completion.
      04-release-notes.md   Generated ONLY at version completion.
  templates/
    *.template.md        Blank versions of the 5 files above, for scaffolding future versions.
  CURRENT.md              Live pointer: which version/step is active right now.
```

## Token-Saving Rule (the most important rule in this file)

**Never re-read `root/roadmap.md` once a version's `00-plan.md` already exists.** Every `00-plan.md` is written to be fully self-contained — goal, theory, sub-steps, and validation criteria are already copied/derived into it. The roadmap is only needed once, when a version's plan is first generated.

**Never re-read a previous version's full `00-plan.md` or `02-outcome.md`.** Each `02-outcome.md` ends with a short **"Context Handoff"** section — a few bullet points listing what now exists (new interfaces, tables, env vars, services) that the *next* version needs to know about. When starting a new version, read **only that handoff section** from the immediately preceding version(s), nothing else.

## Learning-First & Manual Steps Protocol (Essential Mastery)

The primary goal of this repository is ensuring the developer deeply learns the architecture and mechanics hands-on.

1. **Manual Steps are Developer-First**:
   - Items in `01-manual-steps.md` (and any implementation steps providing high learning value) must be presented to the developer with explanations, context, and exact commands rather than being automated away by the agent.
   - The agent guides the developer and waits for them to perform the steps manually.
   - The developer notifies the agent once done; the agent then verifies the work and checks off the items in `01-manual-steps.md` and `00-plan.md`.

2. **Explicit Delegation Exception**:
   - If the developer explicitly instructs the agent to automate or execute manual steps (e.g., "do it for me" or "you do it"), the agent executes the tasks directly and updates the context files accordingly.

3. **Context Sync**:
   - Keep context files in sync with repo state at each sub-step.

## Procedure: Starting a New Version

1. Read `root/RULES.md` (this file) — skip if already in context this session.
2. Read the target version's `00-plan.md` in full.
3. Read the target version's `01-manual-steps.md` in full.
4. Read **only** the "Context Handoff" section from the previous version's `02-outcome.md`.
5. Update `CURRENT.md` to point at this version.
6. Begin work — implement sub-steps from `00-plan.md` in order, one commit per sub-step where practical.

## Procedure: During a Version

- The only files edited are `00-plan.md` (check off sub-steps as completed) and `01-manual-steps.md` (check off manual actions as completed).
- `02-outcome.md`, `03-interview-prep.md`, `04-release-notes.md` stay untouched — no exceptions — until the version is fully done and ready to tag.
- If the plan needs to change mid-version (new info, blocked approach, etc.), edit `00-plan.md` directly and note the change inline — this becomes input for the "deviations from plan" section of `02-outcome.md` later.

## Procedure: Completing a Version

1. Confirm every sub-step in `00-plan.md` and every item in `01-manual-steps.md` is checked.
2. Confirm the plan's Validation criteria are all satisfied.
3. Generate `02-outcome.md`: what was built, deviations from plan + why, theory/code concepts actually applied, alternatives considered, and the **Context Handoff** section for the next version.
4. Generate `03-interview-prep.md`:
   - Likely architecture and implementation questions + short answers + alternative approaches, derived from `02-outcome.md`.
   - **Capture Real Session Confusions**: Actively review the session dialogue from this version. Identify genuine conceptual confusions, doubts, and trade-offs discussed with the developer (e.g. language mechanics, compiler behaviors, dependency flow, architectural trade-offs). Translate these into generalized, high-yield interview questions and concise answers.
5. Generate `04-release-notes.md`: GitHub-ready release description.
6. Update `versions/INDEX.md` — mark this version complete, fill in the completion date.
7. Update `CURRENT.md` to point at the next version.
8. `git tag` the commit using the version folder's slug (e.g. `v08-discord-telegram-channels`).

## Naming Convention

- Version folders: `v{2-digit-number}-{3-5-word-kebab-case-title}` — must match the git tag used for that version exactly.
- Numbers are sequential across the whole roadmap, not reset per phase (e.g. Phase 1 starts at `v05`, not `v01`).

## Phase-Wise Generation

Version folders are created **one phase at a time**, not all 62 up front. When a phase is finished, request scaffolding for the next phase. This keeps the plan files accurate to how the project is actually evolving instead of going stale sitting unused for months.
