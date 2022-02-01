# `tinyballot`, a small web app that implements a simple ballot

This small web app provides some interfaces for creating and managing
simple ballots for general use. At first, it will provide some basic
support for elections under *approval voting*, which is a voting
system where each voter may choose any number of candidates and the
candidate with the most votes wins.

Approval voting aims to maximize overall satisfaction while also
avoiding some of the problems faced by the more common
*first-past-the-post voting* (FPTP). For instance, while under FPTP
similar candidates end up splitting their share of voters, since one
must be necessarily favored over the other; under approval voting
voters can simply choose both, and the results end up reflecting the
candidates' actual approval rating.