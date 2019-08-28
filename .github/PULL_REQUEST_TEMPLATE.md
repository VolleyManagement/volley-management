### Summary

_Describe main changes going into those pull requests._

_Use "closes #(issue number)" to enable GitHub to close issue when PR is merged_

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
- [ ] Test follows [AAA](https://medium.com/@pjbgf/title-testing-code-ocd-and-the-aaa-pattern-df453975ab80) pattern.
- [ ] Test is Isolated
- [ ] Mock instances and expected instances do not share references
- [ ] When new property is added all related comparers are updated

### New Project Checklist

- [ ] Dockerfile is updated
- [ ] Azure Pipelines updated
- [ ] Development setup guide updated
- [ ] VolleyM.API has reference added to assist debugging
