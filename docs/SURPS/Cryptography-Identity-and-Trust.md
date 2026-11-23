# Cryptography, Identity, and Trust — Protecting Claims and Communications

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.Visualize">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop" width="720" />
  </a>
</p>

> **SURPS sourcebook:** secrecy, integrity, identity, authentication, authorization, and trust relationships for education and fictional systems.

## Core distinctions
Cryptography provides tools for protecting information and verifying claims, but it does not automatically make a system trustworthy. Separate confidentiality (who can read), integrity (whether data changed), authenticity (who or what produced a claim), and availability (whether a service can be used).

## Cryptographic concepts
- **Symmetric encryption:** parties use shared secret key material to protect data.
- **Asymmetric cryptography:** related public and private keys support functions such as encryption or digital signatures, depending on the scheme.
- **Hashing:** maps input data to a fixed-size digest; secure hashes are designed to make certain reversals or collisions infeasible.
- **Digital signatures:** support integrity and signer-authentication claims when keys and verification processes are trustworthy.
- **Certificates and trust chains:** bind public keys to identities under an issuer and validation policy.
- **Key management:** generation, distribution, storage, rotation, revocation, backup, and destruction are often harder than the mathematical primitive.

Use vetted standards and libraries in real software; do not invent cryptography. This sourcebook is conceptual and does not prescribe a custom protocol or implementation.

## Identity and access
Authentication establishes which identity is represented. Authorization decides what that identity may do. Accounting records relevant actions. Governance determines who issues identities, approves access, audits use, and resolves disputes. Multifactor authentication, least privilege, separation of duties, and revocation are core defensive ideas.

## Trust is contextual
A valid signature does not prove a statement is true; it supports a claim about integrity and key use. A certificate does not prove a person is benevolent. A logged-in account does not prove the current operator is the account owner. Teach provenance, scope, freshness, compromise, and the difference between technical validity and social trust.

## Fictional game mechanics
Represent keys, certificates, synthetic identities, trust anchors, permissions, and revocation as scenario objects. Puzzles can involve matching an identity claim to an authorized role, checking a fictional certificate chain, discovering an expired credential, or identifying an untrusted issuer. Do not simulate real credential theft, password cracking, key extraction, bypasses, or unauthorized account access.

## Failure and response
Scenarios may include expired certificates, lost devices, misissued identities, compromised synthetic keys, weak governance, stale permissions, unavailable identity services, and clock disagreement. Defensive responses include verification, revocation, rotation, recovery, independent confirmation, and audit.

## LOD
LOD 0: trusted/untrusted claim. LOD 1: identity, credential, and authority roles. LOD 2: trust chains and permission relationships. LOD 3: fictional key lifecycle, audit events, and incident response. LOD 4+: mathematical demonstrations with toy values that cannot be mistaken for production security.


<p align="center">
  <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="420" />
</p>
