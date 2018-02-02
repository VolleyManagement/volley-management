### Summary

_Describe main changes going into those pull requests._

### Areas Of Interest

_Any specific places you want to bring attention of the reviewers? Some places you are uncertain in/some design decisions. (If none remove this section)_

### Checklist

#### Design

- [ ] Web API layer contains as little logic as possible
- [ ] Domain objects use semantic names

#### Perfromance

- [ ] In case of any DB query was changed: attach generated SQL for review
- [ ] Number of DB roundtrips should be as low as possible

#### Unit Tests

- [ ] Naming: Initial state and expected result are properly stated
- [ ] Test follows AAA pattern
- [ ] Test is Isolated
- [ ] Mock instances and expected instances do not share references
