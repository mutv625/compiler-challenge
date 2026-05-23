public class Prop {}

// 命題 True
public sealed class True 
{
    // これが Lean の「True.intro」に相当するコンストラクタ（構成規則）
    public static readonly True Intro = new True();
    
    private True() { } // 外部から別個に作られるのを防ぐ
}

// 命題 False（矛盾）
public sealed class False  // sealed にして継承（自身からの生成）を禁止
{
    private False() { } // インスタンス化を絶対に許さない
    
    // 爆発原理（Falseからは任意の命題が導ける、False.elim）
    public static T Elim<T>(False f) where T : Prop
    {
        // f は絶対に null でしかあり得ず、実体としては存在しない。
        // 論理的な矛盾が起きたため、どんな型 T の値（証明）でも返せると仮定できる。
        throw new Exception("矛盾から任意の命題が導かれました（爆発原理）");
    }
}

// 命題 ¬ P
public sealed class Not<P> where P : Prop
{
    public Func<P, False> Contra { get; }

    // これが Lean の「Not.intro : (P → False) → Not P」に相当
    public Not(Func<P, False> proof)
    {
        Contra = proof;
    }
}

// 命題 P ∧ Q
public sealed class And<P, Q> where P : Prop where Q : Prop
{
    public P Left { get; }
    public Q Right { get; }

    // これが Lean の「And.intro : P → Q → And P Q」に相当
    public And(P p, Q q)
    {
        Left = p;
        Right = q;
    }
}

// 命題 P ∨ Q
public abstract class Or<P, Q> where P : Prop where Q : Prop
{
    private Or() { } // 外部からの勝手な拡張を防ぎ、inl と inr だけに限定する

    // 構成規則①: 左側の証明から Or を作る (Lean の Or.inl)
    public sealed class Inl : Or<P, Q> 
    {
        public P Value { get; }
        public Inl(P p) { Value = p; }
    }

    // 構成規則②: 右側の証明から Or を作る (Lean の Or.inr)
    public sealed class Inr : Or<P, Q>
    {
        public Q Value { get; }
        public Inr(Q q) { Value = q; }
    }

    // 消去規則 / パターンマッチ (Lean の match 式に相当)
    // 「PならばR」という関数と、「QならばR」という関数があれば、
    // 「P ∨ Q」から「R」を取り出すことができる（Orの除去規則）
    public TResult Match<TResult>(Func<P, TResult> leftCase, Func<Q, TResult> rightCase) where TResult : Prop
    {
        if (this is Inl l) return leftCase(l.Value);
        if (this is Inr r) return rightCase(r.Value);
        throw new System.InvalidOperationException("あり得ないケース");
    }
}

class ProofProgram
{
    // 対偶の証明（ (P → Q) → (¬Q → ¬P) 
    Func<Not<Q>, Not<P>> Contrapositive<P, Q>(Func<P,Q> h) where P : Prop where Q : Prop
    {
        Func<Not<Q>, Not<P>> proof = delegate (Not<Q> nq)
        {
            // ¬Q を用いて、P -> Q とぶつけて矛盾を示す
            // P を仮定した下で考える（引数が仮定であるから…）
            Func<P, False> np_prf = delegate (P hp)
            {
                Q q = h(hp);
                False fal = nq.Contra(q);
                return fal;
            };  
            // P を仮定した下で矛盾が示せたため、¬P が構築できる
            Not<P> np = new Not<P>(np_prf);            
            return np;
        };
        // h の仮定の下で ¬Q -> ¬P を構築できた
        return proof;
    }
}