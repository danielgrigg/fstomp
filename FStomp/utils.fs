module utils

let rec translate (rs : (string * string) list) (s : string) = 
    match rs with
    | (r1, r2) :: tail -> translate tail (s.Replace(r1, r2))
    | [] -> s

let consp p h hs = if p then h::hs else hs
